output "connection_string" {
  description = "Connection string for the Azure SQL Database"
  sensitive   = true
  value       = "Host=${docker_container.postgres.name},${var.ports.external};Username=${var.environment_variables.postgres_user};Password=${var.environment_variables.postgres_password};"
}
