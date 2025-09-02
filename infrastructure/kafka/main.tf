resource "docker_image" "kafka" {
  name = "apache/kafka:4.0.0"
}

resource "docker_volume" "data-volume" {
  name   = var.kafka-volume.data
  driver = "local"
}

resource "docker_volume" "logs-volume" {
  name   = var.kafka-volume.logs
  driver = "local"
}

resource "docker_container" "kafka" {
  name  = var.kafka-container.name
  image = docker_image.kafka.image_id

  ports {
    internal = var.kafka-container.ports.internal
    external = var.kafka-container.ports.external
  }

  env = [
    "KAFKA_NODE_ID=${var.kafka-container.node-id}",
    "KAFKA_PROCESS_ROLES=${var.kafka-container.roles}",
    "KAFKA_LISTENERS=${var.kafka-container.listeners}",
    "KAFKA_ADVERTISED_LISTENERS=${var.kafka-container.advertised-listeners}",
    "KAFKA_CONTROLLER_LISTENER_NAMES=${var.kafka-container.controller-listener-names}",
    "KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=${var.kafka-container.listener-security-protocol-map}",
    "KAFKA_CONTROLLER_QUORUM_VOTER=${var.kafka-container.controller-quorum-voters}",
    "KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1",
    "KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR= 1",
    "KAFKA_TRANSACTION_STATE_LOG_MIN_ISR= 1",
    "KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS= 0",
    "KAFKA_NUM_PARTITIONS= 3"
  ]

  volumes {
    volume_name    = docker_volume.data-volume.name
    container_path = "/tmp/kafka-logs"
  }

  volumes {
    volume_name    = docker_volume.logs-volume.name
    container_path = "/opt/kafka/logs"
  }
}
