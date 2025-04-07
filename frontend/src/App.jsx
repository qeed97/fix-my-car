import {StrictMode, useState} from 'react'
import {BrowserRouter, Route, Routes} from "react-router-dom"
import {CookiesProvider, useCookies} from 'react-cookie'
import './style.css'
import MainPage from "./pages/mainPage/MainPage.jsx";

function App() {
  //const [cookies, setCookies] = useCookies(['user']);
  const [searchProblem, setsearchProblem] = useState([]);
  const [normalProblem, setNormalProblem] = useState([]);
/*
  function setUserLoginCookies(user) {
    setCookies('user', user, {path: '/'});
  }
*/
  return (
    <StrictMode>
      <CookiesProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<MainPage searchProblem={searchProblem} normalProblem={normalProblem} setsearchProblem={setsearchProblem} setNormalProblem={setNormalProblem} /*setUserLoginCookies={setUserLoginCookies}*/ />}/>
          </Routes>
        </BrowserRouter>
      </CookiesProvider>
    </StrictMode>
  )
}

export default App
