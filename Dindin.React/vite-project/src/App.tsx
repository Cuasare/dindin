import { BrowserRouter, Routes, Route } from "react-router-dom";
import Register from "./Routes/Register.tsx";

function App() {
  return (
      <BrowserRouter>
        <Routes>
          <Route path="/register" element={<Register />} />
        </Routes>
      </BrowserRouter>
  )
}

export default App;