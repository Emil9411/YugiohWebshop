import React from 'react'
import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import App from './App.jsx'
import './index.css'
import AllCardsPage from './pages/AllCardsPage.jsx'
import MonsterCardsPage from './pages/MonsterCardsPage.jsx'

const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            {
                path: "/all",
                element: <AllCardsPage />,
            },
            {
                path: "/monsters",
                element: <MonsterCardsPage />,
            }
        ],
    },
]
);

const root = ReactDOM.createRoot(document.getElementById('root'));

root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)
