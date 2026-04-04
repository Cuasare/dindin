import { BrowserRouter, Routes, Route } from "react-router-dom";
import Register from "./Routes/Register.tsx";
import Login from "./Routes/Login.tsx";

function App() {
  return (
      <BrowserRouter>
        <Routes>
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
        </Routes>
      </BrowserRouter>
  )
}

export default App;