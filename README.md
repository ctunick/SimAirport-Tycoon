# Python Tooling for SimAirport Tycoon

This directory contains Python scripts for generating game data and performing balance analysis.

## Structure

-   `config/`: YAML configuration files for the data generators.
-   `data_generators/`: Scripts that create game data (flights, passengers).
-   `tools/`: Scripts that process and export data for Unity.

## Getting Started

1.  Create a virtual environment: `python -m venv .venv`
2.  Activate it: `source .venv/bin/activate` (or `.venv\Scripts\activate` on Windows)
3.  Install dependencies: `pip install -r requirements.txt` (if it exists)
4.  Run the exporter: `python tools/export_to_json.py`