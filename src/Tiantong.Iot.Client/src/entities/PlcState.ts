export class PlcState {
  id: number = 0
  plc_id: number = 0
  name: string = ''
  type: string = PlcStateType.uint16
  address: string = ''
  length: number = 0
  is_heartbeat: boolean = false
  heartbeat_interval: number = 1000
  heartbeat_max_value: number = 1000
  is_collect: boolean = false
  collect_interval: number = 1000
  open_log: boolean = false
}

export enum PlcStateType {
  uint16 = 'uint16',
  int32 = 'int32',
  uint32 = 'uint32',
  string = 'string'
}
