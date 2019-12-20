docker-compose -f docker-compose.build.yaml build --no-cache --parallel

if ($? -eq $false) {
  exit
}

docker-compose -f docker-compose.build.yaml up --renew-anon-volumes --abort-on-container-exit outside-in.tests
