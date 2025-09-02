variable "kafka-volume" {
  type = object({
    data = string
    logs = string
  })

  default = {
    data = "kafka-data"
    logs = "kafka-logs"
  }
}

variable "kafka-container" {
  type = object({
    name = string
    ports = object({
      internal = number
      external = number
    })
    node-id                        = string
    roles                          = string
    listeners                      = string
    advertised-listeners           = string
    controller-listener-names      = string
    listener-security-protocol-map = string
    controller-quorum-voters       = string
  })

  default = {
    name = "kafka-quotes"
    ports = {
      internal = 9092
      external = 9092
    }
    node-id                        = 1
    roles                          = "broker, container"
    listeners                      = "PLAINTEXT://0.0.0.0:9092,CONTROLLER://127.0.0.1:9093"
    advertised-listeners           = "PLAINTEXT://localhost:9092"
    controller-listener-names      = "CONTROLLER"
    listener-security-protocol-map = "CONTROLLER:PLAINTEXT,PLAINTEXT:PLAINTEXT"
    controller-quorum-voters       = "1@127.0.0.1:9093"
  }
}

