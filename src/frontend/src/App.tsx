import { useState } from 'react'
import './App.css'

function App() {
  const [value, setValue] = useState<string>("");
  const [result, setResult] = useState<string>("");

  const handleClick = async () => {
    try {
      const response = await fetch(`https://localhost:5000/link/shrink?url=${value}`, {
          method: "POST",
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          }
        });

      const res = await response.json()
      setResult(`https://localhost:5000/${res}`)
    } catch(err: any) {

    } finally {

    }
  }

  return (
    <>
      <div>
        <h1>Shrink link in one click!</h1>
      </div>
      <div className="link-input">
        <input className="link-input__field" type="text" placeholder="www.your-link.com"
          value={value} onChange={(e) => setValue(e.target.value)}/>
        <button className="link-input__action" onClick={handleClick}>
          Shrink
        </button>
        <p className="link-input__tip">
          Enter your link and click button to shorten
        </p>
        <a href={result}>{result}</a>
      </div>
    </>
  )
}

export default App
