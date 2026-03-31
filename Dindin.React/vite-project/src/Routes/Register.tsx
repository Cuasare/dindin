import {useState} from "react";

export default function Register() {
    const [inputType, setInputType] = useState('password')

    const handleMouseDown = () => {
        setInputType('text')
    }

    const handleMouseUp = () => {
        setInputType('password')
    }

    return (
        <div className={"bg-bege-claro flex items-center justify-center min-h-screen"}>

            <div className={"bg-pastel backdrop-blur-md rounded-2xl p-8 shadow-2xl border border-coral-suave/20 h-125 w-100"}>
                <h1 className={"flex justify-center translate-y-20 text-black text-3xl"}>Registrar</h1>
                <div className={"flex flex-col justify-center mt-39"}>
                    <div className={"grid grid-cols-1 gap-3"}>
                        <input type="email" className={"p-2 rounded-md bg-white/45 focus:outline-white mb-2"} placeholder={"Email"}/>
                        <input
                            type={inputType}
                            className={"p-2 rounded-md bg-white/45 focus:outline-white"}
                            placeholder={"Senha"}
                            onMouseDown={handleMouseDown}
                            onMouseUp={handleMouseUp}
                        />
                    </div>
                </div>
            </div>

        </div>
    )
}