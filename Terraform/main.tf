terraform {
  required_providers {
    docker = {
      source = "kreuzwerker/docker"
    }
  }
}

provider "docker" {}

resource "docker_image" "api" {
  name = "rimelsanaa/incidents-api:1.0"
}

resource "docker_container" "server" {
  name  = "devops-server"
  image = docker_image.api.name

  ports {
    internal = 80
    external = 6000
  }

  env = [
    "ASPNETCORE_ENVIRONMENT=Development"
  ]
}