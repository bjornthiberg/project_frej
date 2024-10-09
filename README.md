<div align="center">
  <img src="images/header.png" alt="Frej" width="250" height="250">
  <h1 align="center">Project Frej</h3>
  <p align="center">
    Environmental Data Collection and Visualization App using a Raspberry Pi + ASP.NET Core + React
    <br/>
  </p>
</div>

- [Description](#description)
- [Setup and Installation](#setup-and-installation)
   * [Backend (ASP.NET Core)](#backend-aspnet-core)
   * [Frontend (React)](#frontend-react)
   * [Raspberry Pi Scripts](#raspberry-pi-scripts)
- [Acknowledgements](#Acknowledgements)
- [License](#license)

## Description
Project Frej is an full-stack environmental data collection and visualization system built using Python, ASP.NET Core, and React. It is designed to gather, store, and display data from a Raspberry Pi equipped with an environmental sensor HAT.

<p>
  <img src="images/dashboard.gif" alt="Dashboard gif" width="500">
</p>

The frontend is hosted on [thiberg.dev/project_frej](https://thiberg.dev/project_frej) using GitHub pages, directly from this repository. The ASP.NET Core backend is running in an LXC on my home Promxox server.

Some blog entries journaling the development process can be found on my website: [thiberg.dev](https://thiberg.dev/)

The project includes:
- `rpi_scripts/`(Python): Scripts for data collection, temporary storage, and transmission from the Raspberry Pi.
- `server/`(ASP.NET Core): A Minimal API application using EF Core + SQLite.
- `client/` (React) Uses Material UI and chart.js for some simple data visualization.

## Setup and Installation
First, clone the repository

### Backend (ASP.NET Core)
Requires:
- **.NET SDK**: Version 6.0 or later.
- **SQLite**: For local storage.
```bash
cd server
dotnet restore
dotnet ef database update
dotnet run
```
### Frontend (React)
Requires:
- **Node.js**: Version 14.x or later.
- **npm**: Version 6.x or later. 
```bash
cd client
npm install
npm run dev
```
### Raspberry Pi Scripts
Requires:
- Environmental sensor HAT hardware
- **Python**: 3.7 or later
- **pip**: 20.0 or later 

```bash
cd rpi_scripts
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
python -m main
```

## Acknowledgements
This project uses third-party libraries, each of which may have its own license terms. The list of licenses for most of these libraries can be found in the [LICENSES.md](./LICENSES.md) file.

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE.txt) file for more details.

