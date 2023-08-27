watch:
	dotnet watch --project src/AnagramSolver

docker:
	$(MAKE) docker-build
	$(MAKE) docker-run

docker-run:
	docker run -p 8080:80 --name anagram-solver anagram-solver

docker-build:
	docker build -t anagram-solver .

npm-install:
	cd ./src/AnagramSolver/ClientApp && npm install
	