import React, {useState} from 'react';
import './App.css';
import SyntaxHighlighter from 'react-syntax-highlighter'
import nightOwl from 'react-syntax-highlighter/dist/cjs/styles/hljs/night-owl'

function App() {
  const [error, setError] = useState();
  const [result, setResult] = useState();
  const [input, setInput] = useState('<p>Say hi to <a href="https://portabletext.org"><strong>Portable Text</strong></a> with help of</p> NHI.PortableText');

  const handleSubmit = async (e) => {
    if (e) e.preventDefault()
    if (!input) {
      return
    }
    try {
      const response = await fetch(process.env.REACT_APP_API_ENDPOINT, {
        method: "post",
        body: input,
      })
      const result = await response.json()
      setResult(result)
    } catch (error) {
      setError(error)
    }
  }

  return (
    <main>
      <header>
        <a href="https://github.com/nhi/portable-text-dotnet" target="_blank" rel="noreferrer noopener">
          <img src="https://imagevault.nhi.no/publishedmedia/2lrbbvw7ykb1h82yc92z/Logo-NHI.no.jpg" alt="NHI.no" />
        </a>
        <h1>HTML to PortableText converter (dotnet)</h1>
        <button onClick={handleSubmit}>Convert</button>
      </header>
      {error ? error.message || error : null}
      <form onSubmit={handleSubmit}>
        <textarea value={input} onChange={(event) => setInput(event.target.value)}/>
        <SyntaxHighlighter showLineNumbers style={nightOwl} className="result" language="json">
          {JSON.stringify(result ?? {}, null, 2)}
        </SyntaxHighlighter>
      </form>
    </main>
  );
}

export default App;