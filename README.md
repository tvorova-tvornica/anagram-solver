[![Deploy](https://github.com/ChainsOfPower/anagram-solver/actions/workflows/deploy.yml/badge.svg)](https://github.com/ChainsOfPower/anagram-solver/actions/workflows/deploy.yml)

# anagram-solver

This app allows users to upload wikidata json generated from https://query.wikidata.org/querybuilder/?uselang=en to populate its database and to query anagrams for matching persons available in database.

## backend app
Backend is built using ASP.NET core and Postgres.

## client app
Client is built using React and TS and is deployed with backend as a single unit.

## deployment
App is dockerized and deployed to fly.io

## running app locally
dotnet watch
