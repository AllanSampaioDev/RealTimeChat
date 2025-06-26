<template>
  <div class="login-container">
    <form @submit.prevent="login" class="login-form">
      <h2>Login</h2>

      <input
        type="text"
        placeholder="Digite seu nome"
        v-model="user.name"
        class="login-input"
      />

      <input
        type="password"
        placeholder="Digite sua senha"
        v-model="user.password"
        class="login-input"
      />

      <button type="submit" class="login-button">Entrar</button>

      <p class="info-text">
        <strong>Nota:</strong> Se você ainda não tem uma conta, cadastre-se automaticamente informando nome e senha.
      </p>
    </form>
  </div>
</template>

<script setup>
import { reactive } from 'vue'
import axios from '@/services/http.js'
import { useAuthStore } from '@/stores/auth.js'
import { useToast } from "vue-toastification";
import { useRouter } from 'vue-router'

const toast = useToast()
const router = useRouter()

const user = reactive({
  name: '',
  password: ''
});

const auth = useAuthStore();

async function login() {
  try {
    const {data} = await axios.post('/login', user);
    auth.login(data.token, data);

    toast.success('Login realizado com sucesso!')
    router.push({ name: 'home', params: { user: user } });

  } catch (error) {
    toast.error(error?.response?.data || 'Erro ao realizar login');
  }
}
</script>


<style scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: linear-gradient(to right, #3f87a6, #ebf8e1);
}

.login-form {
  background: white;
  padding: 2rem 3rem;
  border-radius: 10px;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.login-form h2 {
  text-align: center;
  margin-bottom: 1rem;
  color: #333;
}

.login-input {
  padding: 0.75rem 1rem;
  font-size: 1rem;
  border: 1px solid #ccc;
  border-radius: 8px;
  outline: none;
  transition: border-color 0.2s;
}

.login-input:focus {
  border-color: #3f87a6;
}

.login-button {
  padding: 0.75rem;
  font-size: 1rem;
  background-color: #3f87a6;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.login-button:hover {
  background-color: #326c87;
}
</style>