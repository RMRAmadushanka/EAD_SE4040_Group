import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import PrivateRoute from './components/common/PrivateRoute';

import Login from './components/auth/Login';

import Profile from './components/profile/Profile';

import { Register } from './components/auth/Register';
import Home from './components/home/Home';
import { Backoffice } from './components/Pages/Backoffice';
import Travelagent from './components/Pages/Travelagent';
import TravelAccountManagement from './components/Pages/TravelAccountManagement';
import TrainScheduleManagement from './components/Pages/TrainScheduleManagement';

function App() {
    
  return (
    <Router>
      <Routes>
          <Route path="/lk" element={<Login />}/>
          <Route path="/register" element={<Register />}/>  
          <Route exact path="/profile" element={<Profile />} />
          <Route exact path="/backoffice" element={<Backoffice />} />
          <Route exact path="/travelagent" element={<Profile />} />
          <Route exact path="/sds" element={<TravelAccountManagement />} />
          <Route exact path="/" element={<TrainScheduleManagement />} />
      </Routes>
         
    </Router>
  );
}

export default App;
