import { DateTime } from "@midos/vue-ui";

export class PlcStateLog {
  id = 0

  plc_id = 0

  state_id = 0

  operation = ""

  value = ""

  created_at = DateTime.minValue
}
