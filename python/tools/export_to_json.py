# python/tools/export_to_json.py

import json
from pathlib import Path


def get_unity_data_dir() -> Path:
    """
    Resolve the Unity Data/ directory relative to this repo layout:

    SimAirport-Tycoon/
      Assets/Data/
      python/
        tools/export_to_json.py
    """
    # export_to_json.py -> tools -> python -> repo_root
    repo_root = Path(__file__).resolve().parents[2]
    data_dir = repo_root / "Assets" / "Data"
    data_dir.mkdir(parents=True, exist_ok=True)
    return data_dir


def main() -> None:
    data_dir = get_unity_data_dir()

    schedule = {
        "flights": [
            {
                "flightNumber": "SA1001",
                "origin": "LAX",
                "destination": "JFK",
                "departureTimeLocal": "06:00",
                "arrivalTimeLocal": "14:30",
                "gate": "A1",
            }
        ]
    }

    out_path = data_dir / "sample_schedule.json"
    with out_path.open("w", encoding="utf-8") as f:
        json.dump(schedule, f, indent=2)

    print(f"Wrote stub schedule to {out_path}")


if __name__ == "__main__":
    main()
