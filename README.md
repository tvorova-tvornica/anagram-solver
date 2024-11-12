<p align="center">
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/2048px-.NET_Core_Logo.svg.png" width="100" />
  <img src="https://cdn.iconscout.com/icon/free/png-256/free-react-logo-icon-download-in-svg-png-gif-file-formats--technology-social-media-vol-5-pack-logos-icons-2945110.png?f=webp&w=256" width="100">
</p>
<p align="center">
    <h1 align="center">Anagram Solver</h1>
</p>
<p align="center">
    <img src="https://img.shields.io/badge/license-MIT-blue" alt="license">
	<img src="https://img.shields.io/github/last-commit/tvorova-tvornica/anagram-solver?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/tvorova-tvornica/anagram-solver?style=flat&color=0080ff" alt="repo-top-language">
	<img src="https://img.shields.io/github/languages/count/tvorova-tvornica/anagram-solver?style=flat&color=0080ff" alt="repo-language-count">
<p>
<hr> 

##  Quick Links

> - [ Overview](#overview)
> - [ Features](#features)
> - [ Project Structure](#project-structure)
> - [ Getting Started](#getting-started)

---

## Overview

The goal of this project is to be able to solve any anagram of famous person. <br><br> We attend a pub quiz once a week and one of the questions is to solve anagram. <br><br> That got us wonder what would take to create a web that does just that, and to be able to solve anagram of any famous person.<br> We decided to tackle that challenge and to create this project as a fun side-project.

--- 
## Features
There are three main modules of this app, namely 
- As admin, request import of famous people from wikidata, by just providing occupation id
- As admin, import person manually
- As anonymous user, enter anagram in input field and immediatelly get solution

---
##  Project Structure
Project is structured as ASP.NET monolith.

In development, backend automatically starts react app and configures the proxy so that react app consumes mvc controllers.

In production build, react app is built and bundled as APS.NET static asset, being served from it's index page.

---

##  Getting Started

***Requirements***

Ensure you have the following dependencies installed on your system:

* **dotnet SDK**: `version 7.0 or higher`
* **Node.js** `version 14.0.0 or higher`

###  Running locally

1. Clone the Anagram Solver repository:

```sh
git clone git@github.com:tvorova-tvornica/anagram-solver.git
```

2. Change to the project directory:

```sh
cd anagram-solver
```

3. Run the app in watch mode:

```sh
dotnet watch
```
---
