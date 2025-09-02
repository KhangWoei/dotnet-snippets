variable "database-volume" {
  type    = string
  default = "database-quotes-volume"
}

variable "database-container" {
  type = object({
    container-name = string
    ports = object({
      internal = number
      external = number
    })
    user          = string
    password      = string
    database-name = string
    data-path     = string
  })

  default = {
    container-name = "database-quotes",
    ports = {
      internal = 5432
      external = 5432
    }
    user          = "postgres"
    password      = null
    database-name = "postgres"
    data-path     = "/var/lib/postgresql/data"
  }
}
