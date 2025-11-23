# python/data_generators/fake_flight_schedule.py
import json
import os

def generate_fake_schedule():
    """
    Generates a hardcoded, minimal flight schedule.
    
    TODO: Implement logic to pull from a real flight data API (e.g., FlightAware).
    TODO: Add randomization for times, gates, and aircraft types.
    """
    return [
      {
        "flight_number": "SA123",
        "airline": "SimAir",
        "origin": "SIM-LAX",
        "destination": "SIM-JFK",
        "scheduled_departure": "2025-11-22T08:00:00Z",
        "scheduled_arrival": "2025-11-22T16:00:00Z",
        "gate": "A1",
        "aircraft_type": "SimBus A320"
      },
      {
        "flight_number": "FT456",
        "airline": "FlyTycoon",
        "origin": "SIM-ORD",
        "destination": "SIM-MIA",
        "scheduled_departure": "2025-11-22T09:30:00Z",
        "scheduled_arrival": "2025-11-22T13:00:00Z",
        "gate": "B3",
        "aircraft_type": "SimJet 737"
      },
      {
        "flight_number": "SA789",
        "airline": "SimAir",
        "origin": "SIM-SFO",
        "destination": "SIM-LAX",
        "scheduled_departure": "2025-11-22T11:00:00Z",
        "scheduled_arrival": "2025-11-22T12:00:00Z",
        "gate": "A2",
        "aircraft_type": "SimBus A321"
      }
    ]

def main():
    """
    Main function to generate and save the sample flight schedule.
    """
    config_path = os.path.join(os.path.dirname(__file__), '..', 'config', 'sample_flight_schedule.json')
    
    print("Generating fake flight schedule...")
    schedule = generate_fake_schedule()
    
    print(f"Writing schedule to {config_path}...")
    with open(config_path, 'w') as f:
        json.dump(schedule, f, indent=2)
    
    print("Done.")

if __name__ == "__main__":
    main()
