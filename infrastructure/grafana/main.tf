resource "docker_image" "grafana" {
  name = "grafana/grafana"
}

resource "docker_volume" "volume" {
  name   = var.grafana-volume
  driver = "local"
}

resource "docker_container" "grafana" {
  name  = var.grafana-container.container-name
  image = docker_image.grafana.image_id

  ports {
    internal = 3000
    external = var.grafana-container.port
  }

  env = [
    "GF_SERVER_ROOT_URL=${var.grafana-container.server-url}",
    "GF_PLUGINS_PREINSTALL=${var.grafana-container.plugins}"
  ]

  volumes {
    volume_name    = docker_volume.volume.name
    container_path = "/var/lib/grafana"
  }
}
