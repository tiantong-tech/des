<template>
  <div class="is-flex-auto is-flex">
    <aside
      class="menu is-unselectable"
      style="min-width: 260px; max-width: 260px"
    >
      <ul class="menu-list">
        <li>
          <router-link
            v-for="(menu, key) in menus" :key="key"
            :to="{ name: menu.route }"
          >
            <span class="icon">
              <i :class="`icon-api-gateway icon-api-gateway-${menu.icon}`"></i>
            </span>
            <span>{{menu.text}}</span>
          </router-link>
        </li>
      </ul>
    </aside>

    <router-view class="is-flex-auto" style="overflow-y: auto"/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "App",

  setup() {
    return {
      menus: [
        { text: "运行状态", route: "ApiGatewayDashboard", icon: "menu" },
        { text: "服务管理", route: "ApiGatewayEndpoints", icon: "menu" },
        { text: "路由管理", route: "ApiGatewayRoutes", icon: "menu" },
        { text: "设置", route: "ApiGatewaySettings", icon: "menu" },
      ]
    };
  }
});
</script>
