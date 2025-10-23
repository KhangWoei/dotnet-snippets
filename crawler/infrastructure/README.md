# Getting started

## 1. Initializing terraform

Initializes the directory containing Terraform configuration files. Downloads providers, downloads modules, and initializes the backend resources.

```
terraform init
```

## 2. Creating the database

### 2.1 Dry-run

```
terraform plan -var 'ports={internal=<internal>, external=<external>}' -out '<plan_file_name>
```

### 2.2 Actual
Executes changes defined in the `main.tf` file.

```
terraform apply 

#### OR ####

terraform apply '<plan_file_name>'
```

## 3. Getting terraform outputs

```
terraform output --raw '<output_vars>'
```

## 4. Cleanup

### 4.1 Dry-run

```
terraform plan -destroy -var '...' -out '<plan_file_name>'
```

### 4.2 Actual

```
terraform apply --destroy

#### OR ####

terraform apply --destroy  '<plan_file_name>'
```
