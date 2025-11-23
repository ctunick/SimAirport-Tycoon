import random

def generate_flight_schedule(config):
    """
    Generates a simple list of flights for a day.
    """
    schedule = []
    flight_count = config['flights']['count']
    total_minutes = config['simulation_day_length_hours'] * 60

    for i in range(flight_count):
        schedule.append({
            "flight_id": f"AT{100 + i}",
            "time_minutes": (i * (total_minutes // flight_count)) + random.randint(-15, 15),
            "gate": "GateA", # Hardcoded for v0.0.1
            "passengers": random.randint(config['flights']['passengers_per_flight'][0], config['flights']['passengers_per_flight'][1])
        })

    return schedule