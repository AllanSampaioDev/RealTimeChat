<template>
  <div class="messages-wrapper">
    <div class="messages-scroll" ref="scrollContainer">
      <div v-for="(message, index) in messages" :key="index">
        <q-chat-message
          v-if="message.senderUserId == userId"
          :text="[message.body]"
          :name="userName"
          :stamp="message.sendTime"
          size="4"
          bg-color="teal-5"
          sent
        />
        <q-chat-message
          v-else
          :text="[message.body]"
          :name="contactName"
          :stamp="message.sendTime"
          text-color="white"
          size="4"
          bg-color="blue-grey-8"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { nextTick } from 'vue'

export default {
  name: 'ChatComponent',
  props: {
    userId: String,
    userName: String,
    contactName: String,
    messages: Array
  },
  watch: {
    messages() {
      this.scrollToBottom()
    }
  },
  mounted() {
    this.scrollToBottom()
  },
  methods: {
    scrollToBottom() {
      nextTick(() => {
        const container = this.$refs.scrollContainer
        if (container) {
          container.scrollTop = container.scrollHeight
        }
      })
    }
  }
}
</script>

<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
.messages-wrapper {
  flex: 1;
  display: flex;
  flex-direction: column;
  height: 100%;
}

.messages-scroll {
  flex: 1;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  padding: 0.5rem;
}
</style>
