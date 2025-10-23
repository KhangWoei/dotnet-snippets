variable "grafana-volume" {
  type    = string
  default = "grafana-volume"
}

variable "grafana-container" {
  type = object({
    container-name = string
    port           = number
    server-url     = string
    plugins        = string
  })

  default = {
    container-name = "grafana",
    port           = 3000,
    server-url     = "http://quotes.grafana/",
    plugins        = ""
  }
}
