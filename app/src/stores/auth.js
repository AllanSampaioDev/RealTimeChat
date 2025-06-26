import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: null,
    user: null
  }),
  actions: {
    login(newToken, newUser) {
      localStorage.setItem('token', newToken)
      localStorage.setItem('user', JSON.stringify(newUser))
      this.token = newToken
      this.user = newUser
    },
    logout() {
      this.token = null
      this.user = null
    }
  },
  persist: true 
})
