import React, {useState, useEffect} from 'react';
import './App.css';
import BlockContent from "@sanity/block-content-to-react"
import axios from "axios"


function App() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [portableText, setPortableText] = useState([]);
  const [convertedPortableText, setInputPortableText] = useState();
  const [textAreaString, setTextAreaString] = useState();

  useEffect(() => {
    const body = '<p>Say hi to <a href="https://portabletext.org"><strong>Portable Text</strong></a> with help of</p>'
    axios.post(
      process.env.REACT_APP_API_ENDPOINT, 
      body,
      {
        headers: {
          'Content-Type': 'text/plain'
        }
      })
      .then(result => {
        setPortableText(result.data);
        setIsLoaded(true);
      })
      .catch(error => {
        setInputPortableText(undefined)
        setIsLoaded(true);
        setError(error);
      })
  }, [])

  const postFunction = (e) => {
    e.preventDefault()
    axios.post(
      process.env.REACT_APP_API_ENDPOINT, 
      textAreaString,
      {
        headers: {
          'Content-Type': 'text/plain'
        }
      })
      .then(result => {
        setInputPortableText(result.data);
        setIsLoaded(true);
      })
      .catch(error => {
        setIsLoaded(true);
        setError(error);
      })
  }

  const handleChange = (event) => {
    setTextAreaString(event.target.value)
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <div>Loading...</div>;
  } else {
    return (
      <div className="App">
        <header className="App-header">
          <BlockContent blocks={portableText} />
          <img src="https://imagevault.nhi.no/publishedmedia/2lrbbvw7ykb1h82yc92z/Logo-NHI.no.jpg" alt="NHI.no" />
          <form onSubmit={postFunction}>
            <label>
              HTMLString:
              <textarea value={textAreaString} onChange={handleChange} />
            </label>
            <input type="submit" value="Submit" />
          </form>
    <pre>{JSON.stringify(convertedPortableText, null, 2)}</pre>
        </header>
      </div>
    );
  }
}

export default App;