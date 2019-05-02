workflow "New workflow" {
  on = "pull_request"
  resolves = ["GraphQL query"]
}

action "GitHub Action for Docker" {
  uses = "actions/docker/cli@8cdf801b322af5f369e00d85e9cf3a7122f49108"
}

action "GraphQL query" {
  uses = "helaili/github-graphql-action@fb0ce78d56777b082e1a1659faf2b9f5a8832ed3"
  needs = ["GitHub Action for Docker"]
}
