# CHANGELOG.md

## [2024-02-04]
Command to create postgres container: 
``` bash
docker run -d --name tododb \
-e POSTGRES_PASSWORD=password \
-e PGDATA=/var/lib/postgresql/data/pgdata \
-v /Users/brian/Documents/docker_data/todo_db:/var/lib/postgresql/data \
-p 5432:5432 \
postgres:16.1
```
## [2023-12-27]
Feature: 
- Config db migration - grate
- Install psql to local machine to run the script. (Can move it to docker in future)
- Create `user` and `task` tables.

