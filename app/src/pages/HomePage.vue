<template>
  <div class="container">
    <div class="users-list">
      <h3>Contatos</h3>
      <ul>
        <li 
          v-for="contact in usersOnline.filter(u => u.userId !== user.id)" 
          :key="contact.userId" 
          @click="openChat(contact.userName, contact.userId)"
          :class="{ active: contact.userName === contactName }"
        >
          <span 
            class="status-dot" 
            :class="{ connected: contact.connected, disconnected: !contact.connected }"
          ></span>
          {{ contact.userName }}
        </li>
      </ul>
    </div>

    <div class="home-container">
      <div class="chat-box">
        <div class="user-header">
          <div v-if="contactName" class="chat-with">Chat com: <strong>{{ contactName }}</strong></div>
        </div>

        <div class="chat-area">
          <ChatComponent
            v-if="user && user.name"
            :messages="messages"
            :userId="user.id"
            :userName="user.name"
            :contactName="contactName || ''"
            style="flex: 1"
          />
        </div>

        <form @submit.prevent="send" class="chat-input-area">
          <input
            type="text"
            v-model="message.body"
            placeholder="Digite sua mensagem..."
            class="chat-input"
            :disabled="!contactId"
          />
          <button type="submit" :disabled="!contactId" class="chat-send-button">Enviar</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import ChatComponent from '../components/ChatComponent.vue'
import { ref, reactive, onMounted } from 'vue'
import { useToast } from "vue-toastification";
import axios from '@/services/http.js'
import Hub from '../Hub'

const toast = useToast()

export default {
  name: 'HomePage',
  components: {
    ChatComponent
  },

  setup() {
    const user = reactive(JSON.parse(localStorage.getItem('user')) || {})

    let contactId = ref('')
    let contactName = ref('')
    let usersOnline = ref([])
    let messages = ref([])
    let message = reactive({
      senderUserId: user.id,
      recieverUserId: '',
      body: '',
      sendTime: new Date().toLocaleTimeString()
    })

    let _hub = new Hub(user.id, user.name)

    async function openChat(contact_name, contact_id) {
      contactId.value = contact_id;
      contactName.value = contact_name;
      messages.value = [];

      try {
        const {data} = await axios.get(`/messages/${user.id}/${contact_id}`);
     
        messages.value = data;
      } catch (error) {
        toast.error(error?.response?.data || 'Erro ao carregar histórico');
      }
    }

    function send() {
      if (user.name && message.body && contactName.value) {
        message.recieverUserId = contactId.value 
        message.sendTime = new Date().toLocaleString()

        _hub.connection.invoke('SendMessageToUser', contactId.value, message).then(() => {
          messages.value.push({...message}) 
          message.body = ''
        })
        .catch(err => toast.error('Error while sending message: ' + err))
      } else {
        toast.error('Preencha destinatário, nome e mensagem.')
      }
    }

    onMounted(() => {
      _hub.connection.start()
      .then(() => {
        console.log('Connection started')

        _hub.connection.on('UsersOnline', (users) => {
          usersOnline.value = users
        })

        _hub.connection.on('ReceivePrivateMessage', (msg) => {
          messages.value.push(msg)
        })
      })
      .catch(err => toast.error('Error while starting connection: ' + err))
    })

    return {
      user,
      contactName,
      usersOnline,
      send,
      messages,
      message,
      openChat,
      contactId
    }
  }
}
</script>

<style scoped>
.container {
  display: flex;
  height: 100vh;
  padding: 1rem;
  background: linear-gradient(to right, #3f87a6, #ebf8e1);
}

.users-list {
  width: 250px;
  background: white;
  border-radius: 10px;
  padding: 1rem;
  margin-right: 1rem;
  overflow-y: auto;
}

.users-list ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.users-list li {
  padding: 0.5rem;
  cursor: pointer;
  border-radius: 5px;
}

.users-list li.active,
.users-list li:hover {
  background-color: #3f87a6;
  color: white;
}

.home-container {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.chat-box {
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  flex: 1;
  min-height: 0;
}

.user-header {
  text-align: center;
  color: #333;
}

.chat-with {
  margin-top: 0.5rem;
  font-weight: bold;
  color: #3f87a6;
}

.chat-area {
  flex: 1;
  min-height: 0;
  overflow: hidden;
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 0;
  padding-left: 1rem;
  padding-right: 1rem;
  background-color: #f9f9f9;
  display: flex;
  flex-direction: column;
}


.chat-input-area {
  display: flex;
  gap: 0.5rem;
  align-items: center;
}

.chat-input {
  flex: 1;
  padding: 0.75rem 1rem;
  font-size: 1rem;
  border: 1px solid #ccc;
  border-radius: 8px;
  outline: none;
  transition: border-color 0.2s;
}

.chat-input:focus {
  border-color: #3f87a6;
}

.chat-send-button {
  padding: 0.75rem 1.25rem;
  background-color: #3f87a6;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: bold;
  transition: background-color 0.2s;
}

.chat-send-button:hover {
  background-color: #326c87;
}
.status-dot {
  display: inline-block;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  margin-right: 8px;
  vertical-align: middle;
}

.connected {
  background-color: #4caf50;
}

.disconnected {
  background-color: #f44336;
}

</style>
