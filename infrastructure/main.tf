resource "docker_image" "postgres" {
  name = "postgres:latest"
}

resource "docker_container" "postgres" {
  name  = "crawl_db"
  image = docker_image.postgres.image_id
  ports {
    internal = var.ports.internal
    external = var.ports.external
  }
  env = [
    "POSTGRES_USER=${var.environment_variables.postgres_user}",
    "POSTGRES_PASSWORD=${var.environment_variables.postgres_password}",
    "POSTGRES_DB=${var.environment_variables.postgres_db}",
    "POSTGRES_INITDB_ARGS=${var.environment_variables.postgres_initdb_args}",
    "POSTGRES_INITDB_WALDIR=${var.environment_variables.postgres_initdb_waldir}",
    "POSTGRES_HOST_AUTH_METHOD=${var.environment_variables.postgres_host_auth_method}",
    "PG_DATA=${var.environment_variables.pg_data}"
  ]
}

