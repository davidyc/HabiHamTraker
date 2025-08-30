import { useState, useEffect } from 'react'
import { weightService } from './services/weightService'
import './App.css'

function App() {
  const [weightEntries, setWeightEntries] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)
  const [dateFrom, setDateFrom] = useState('')
  const [dateTo, setDateTo] = useState('')
  const [entryDate, setEntryDate] = useState(new Date().toISOString().split('T')[0])
  const [entryWeight, setEntryWeight] = useState('')
  const [addingEntry, setAddingEntry] = useState(false)

  const fetchWeightEntries = async () => {
    setLoading(true)
    setError(null)
    
    try {
      const from = dateFrom ? new Date(dateFrom) : null
      const to = dateTo ? new Date(dateTo) : null
      
      const data = await weightService.getAll(from, to)
      setWeightEntries(data)
    } catch (err) {
      setError(err.message)
      console.error('Error fetching weight entries:', err)
    } finally {
      setLoading(false)
    }
  }

  const handleAddEntry = async () => {
    if (!entryWeight) {
      setError('Please enter a weight value')
      return
    }

    setAddingEntry(true)
    setError(null)
    
    try {
      const newEntry = {
        date: new Date(entryDate),
        weightKg: parseFloat(entryWeight)
      }
      
      await weightService.create(newEntry)
      
      // Reset form and refresh entries
      setEntryWeight('')
      setEntryDate(new Date().toISOString().split('T')[0])
      await fetchWeightEntries()
    } catch (err) {
      setError(err.message)
      console.error('Error adding weight entry:', err)
    } finally {
      setAddingEntry(false)
    }
  }

  useEffect(() => {
    fetchWeightEntries()
  }, [])

  return (
    <div className="app">
      <h1>HabiHam Weight Tracker</h1>
      
      <div className="add-entry">
        <h3>Add New Weight Entry</h3>
        <div className="entry-form">
          <div className="form-group">
            <label htmlFor="entryDate">Date:</label>
            <input
              type="date"
              id="entryDate"
              value={new Date().toISOString().split('T')[0]}
              onChange={(e) => setEntryDate(e.target.value)}
            />
          </div>
          <div className="form-group">
            <label htmlFor="entryWeight">Weight (kg):</label>
            <input
              type="number"
              id="entryWeight"
              step="0.1"
              placeholder="Enter weight"
              onChange={(e) => setEntryWeight(e.target.value)}
            />
          </div>
          <button onClick={handleAddEntry} disabled={addingEntry}>
            {addingEntry ? 'Adding...' : 'Add Entry'}
          </button>
        </div>
      </div>

      <div className="filters">
        <div className="filter-group">
          <label htmlFor="dateFrom">From Date:</label>
          <input
            type="date"
            id="dateFrom"
            value={dateFrom}
            onChange={(e) => setDateFrom(e.target.value)}
          />
        </div>
        
        <div className="filter-group">
          <label htmlFor="dateTo">To Date:</label>
          <input
            type="date"
            id="dateTo"
            value={dateTo}
            onChange={(e) => setDateTo(e.target.value)}
          />
        </div>
        
        <button onClick={fetchWeightEntries} disabled={loading}>
          {loading ? 'Loading...' : 'Filter Entries'}
        </button>
      </div>

      {error && (
        <div className="error">
          Error: {error}
        </div>
      )}

      {loading ? (
        <div className="loading">Loading weight entries...</div>
      ) : (
        <div className="weight-entries">
          <h2>Weight Entries</h2>
          {weightEntries.length === 0 ? (
            <p>No weight entries found.</p>
          ) : (
            <div className="entries-list">
              {weightEntries.map((entry) => (
                                 <div key={entry.id} className="entry-card">
                   <div className="entry-date">
                     {new Date(entry.date).toLocaleDateString()}
                   </div>
                   <div className="entry-weight">
                     {entry.weightKg} kg
                   </div>
                   <div className="entry-created">
                     Created: {new Date(entry.createdAt).toLocaleDateString()}
                   </div>
                 </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  )
}

export default App
