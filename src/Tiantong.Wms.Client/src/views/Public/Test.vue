<template>
  <div>
    Test
    {{value}}
    {{test}}
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

class Test extends Vue {
  value = 100

  params!: string

  get test() {
    return 2000
  }
}

@Component({
  name: 'test'
})
export default class extends Test {
  value = 1000

  handle (value: never) {
    console.log(value)
  }

  created () {
    this.handle(1000 as never)
  }
}

</script>
