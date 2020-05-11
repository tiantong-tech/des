import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from '@/views/_routes'

Vue.use(VueRouter)

export default new VueRouter({
  routes,
  linkActiveClass: 'is-active'
})
