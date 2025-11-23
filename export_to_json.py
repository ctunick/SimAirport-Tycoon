import json
import os
import yaml
from data_generators.flight_schedule_generator import generate_flight_schedule
from data_generators.passenger_profile_generator import generate_passenger_profiles

def main():
    """
    Main export function. Reads configs, calls generators, and writes JSON files
    to the Unity project's data folder.
    """
    config_path = os.path.join('..', 'config', 'example_airport_config.yaml')
    unity_data_path = os.path.join('..', '..', 'unity', 'AirportTycoonUnity', 'Assets', 'Data')

    print(f"Loading config from {config_path}...")
    with open(config_path, 'r') as f:
        config = yaml.safe_load(f)

    # Generate flight schedule
    flight_schedule = generate_flight_schedule(config)
    schedule_data = {
        "airport_id": config['airport_id'],
        "flight_schedule": flight_schedule
    }

    # Write to JSON
    output_filename = os.path.join(unity_data_path, 'flight_schedule.json')
    os.makedirs(os.path.dirname(output_filename), exist_ok=True)
    with open(output_filename, 'w') as f:
        json.dump(schedule_data, f, indent=2)
    print(f"Wrote flight schedule to {output_filename}")

if __name__ == "__main__":
    main()