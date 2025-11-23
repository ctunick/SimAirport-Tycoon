def generate_passenger_profiles():
    """
    Generates a simple list of passenger profiles.
    For v0.0.1, this is just a stub.
    """
    profiles = [
        {"profile_id": "business_traveler", "walking_speed_modifier": 1.2},
        {"profile_id": "leisure_traveler", "walking_speed_modifier": 0.9},
        {"profile_id": "family_traveler", "walking_speed_modifier": 0.7},
    ]
    return profiles