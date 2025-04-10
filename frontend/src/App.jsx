import {StrictMode, useState} from 'react';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {useCookies} from 'react-cookie';
import './style.css';
import MainPage from "./pages/mainPage/MainPage.jsx";
import RegisterPage from "./pages/registerPage/RegisterPage.jsx";
import LoginPage from "./pages/loginPage/LoginPage.jsx";
import {Toaster} from "react-hot-toast";

export default function App() {
  const [cookies, setCookies] = useCookies(['user']);
  const [searchProblem, setsearchProblem] = useState([]);
  const [normalProblem, setNormalProblem] = useState([]);

  function setUserLoginCookies(user) {
    setCookies('user', user, {path: '/'});
  }

  return (
    <StrictMode>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<MainPage searchProblem={searchProblem} normalProblem={normalProblem} setsearchProblem={setsearchProblem} setNormalProblem={setNormalProblem} /*setUserLoginCookies={setUserLoginCookies}*/ />}/>
            <Route path="/register" element={<RegisterPage setUserLoginCookies={setUserLoginCookies}/>}/>
            <Route path="/login" element={<LoginPage setUserLoginCookies={setUserLoginCookies}/>}/>
          </Routes>
          <Toaster/>
        </BrowserRouter>
    </StrictMode>
  )
}
