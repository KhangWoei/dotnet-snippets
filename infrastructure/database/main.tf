resource "docker_image" "database" {
  name = "timescale/timescaledb-ha:pg17"
}

resource "docker_volume" "volume" {
  name   = var.database-volume
  driver = "local"
}

resource "random_password" "password" {
  count            = var.database-container.password == null ? 1 : 0
  length           = 20
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
  min_numeric      = 1
  min_upper        = 1
  min_lower        = 1
  min_special      = 1

}

resource "docker_container" "database" {
  name  = var.database-container.container-name
  image = docker_image.database.image_id

  ports {
    internal = var.database-container.ports.internal
    external = var.database-container.ports.external
  }

  env = [
    "POSTGRES_USER=${var.database-container.user}",
    "POSTGRES_PASSWORD=${try(random_password.password[0].result, var.database-container.password)}",
    "POSTGRES_DB=${var.database-container.database-name}",
    "PGDATA=${var.database-container.data-path}"
  ]

  volumes {
    volume_name    = docker_volume.volume.name
    container_path = var.database-container.data-path
  }
}
