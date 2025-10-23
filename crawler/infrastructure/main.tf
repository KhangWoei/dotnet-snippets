resource "docker_image" "postgres" {
  name         = "postgres:17"
  keep_locally = true
}

resource "docker_volume" "postgres_data" {
  name = "${var.database_name}_data"
}

resource "docker_container" "postgres" {
  name  = var.database_name
  image = docker_image.postgres.name
  ports {
    internal = var.ports.internal
    external = var.ports.external
    protocol = "tcp"
  }
  env = [
    "POSTGRES_USER=${var.environment_variables.postgres_user}",
    "POSTGRES_PASSWORD=${var.environment_variables.postgres_password}",
    "POSTGRES_DB=${var.environment_variables.postgres_db}",
    "POSTGRES_INITDB_ARGS=${var.environment_variables.postgres_initdb_args}",
    "POSTGRES_INITDB_WALDIR=${var.environment_variables.postgres_initdb_waldir}",
    "POSTGRES_HOST_AUTH_METHOD=${var.environment_variables.postgres_host_auth_method}",
    "PGDATA=${var.environment_variables.pg_data}/pgdata"
  ]
  volumes {
    volume_name    = docker_volume.postgres_data.name
    container_path = var.environment_variables.pg_data
  }
}

