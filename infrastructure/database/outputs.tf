output "connection_string" {
  value     = "Host=${docker_container.database.name},${docker_container.database.ports[0].external};User ID=${var.database-container.user};Password=${try(random_password.password[0].result, var.database-container.password)};Database=${var.database-container.database-name}"
  sensitive = true
}
