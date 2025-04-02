import {StrictMode, useState} from 'react'
import {BrowserRouter, Route, Routes} from "react-router-dom"
import {CookiesProvider, useCookies} from 'react-cookie'
import './style.css'

function App() {
  const [cookies, setCookie] = useCookies(['user'])
  

  return (
    <StrictMode>

    </StrictMode>
  )
}

export default App
