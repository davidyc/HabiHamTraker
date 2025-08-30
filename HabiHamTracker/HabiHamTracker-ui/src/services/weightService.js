const API_BASE_URL = 'http://localhost:5000/api/weight';

export const weightService = {
  async getAll(from = null, to = null) {
    const params = new URLSearchParams();
    if (from) params.append('from', from.toISOString());
    if (to) params.append('to', to.toISOString());
    
    const url = `${API_BASE_URL}?${params.toString()}`;
    const response = await fetch(url);
    
    if (!response.ok) {
      throw new Error(`Failed to fetch weight entries: ${response.statusText}`);
    }
    
    return response.json();
  },

  async getById(id) {
    const response = await fetch(`${API_BASE_URL}/${id}`);
    
    if (!response.ok) {
      throw new Error(`Failed to fetch weight entry: ${response.statusText}`);
    }
    
    return response.json();
  },

  async getLatest() {
    const response = await fetch(`${API_BASE_URL}/latest`);
    
    if (!response.ok) {
      throw new Error(`Failed to fetch latest weight entry: ${response.statusText}`);
    }
    
    return response.json();
  },

  async getSummary(days = 30) {
    const response = await fetch(`${API_BASE_URL}/summary?days=${days}`);
    
    if (!response.ok) {
      throw new Error(`Failed to fetch summary: ${response.statusText}`);
    }
    
    return response.json();
  },

  async create(entry) {
    const response = await fetch(API_BASE_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(entry),
    });
    
    if (!response.ok) {
      throw new Error(`Failed to create weight entry: ${response.statusText}`);
    }
    
    return response.json();
  }
};
