import './App.css'

function App() {
  return (
    <>
      <div>
        <h1>Shrink link in one click!</h1>
      </div>
      <div className="link-input">
        <input className="link-input__field" type="text" placeholder="www.your-link.com"/>
        <button className="link-input__action">
          Shrink
        </button>
        <p className="link-input__tip">
          Enter your link and click button to shorten
        </p>
      </div>
    </>
  )
}

export default App
