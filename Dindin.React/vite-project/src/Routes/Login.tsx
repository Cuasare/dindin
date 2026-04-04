import { Eye, EyeClosed, LoaderCircle } from "lucide-react";
import axios from "axios";
import {useEffect, useState} from "react";
import * as EmailValidator from "email-validator";

type FormErrors = {
    email?: string;
    password?: string;
};

type FormData = {
    email: string;
    password: string;
};

export default function Login() {
    const [isPosting, setIsPosting] = useState(false);
    const [inputType, setInputType] = useState<"password" | "text">("password");
    const [response, setResponse] = useState("");
    const [isError, setIsError] = useState(false)
    const [formErrors, setFormErrors] = useState<FormErrors>({});
    const [formData, setFormData] = useState<FormData>({
        email: "",
        password: "",
    });

    const handleMouseDown = () => setInputType("text");
    const handleMouseUp = () => setInputType("password");

    const handleButtonClick = async () => {
        const newErrors: FormErrors = {};
        if (!EmailValidator.validate(formData.email)) {
            newErrors.email = "Email inválido";
        }
        if (formData.password.length < 8) {
            newErrors.password = "Senha incorreta!";
        }

        setFormErrors(newErrors);

        if (Object.keys(newErrors).length > 0) return;

        setIsPosting(true);
        await sendData();
    };

    async function sendData() {
        try {
            const resposta = await axios.post(
                "http://localhost:8080/api/auth/login",
                {
                    email: formData.email,
                    password: formData.password,
                },
                { withCredentials: true }
            );

            setResponse(resposta.data);

            useEffect(() => {
                localStorage.setItem("acessToken", JSON.stringify(response))
            }, [response]);

        } catch (error) {
            if (axios.isAxiosError(error)) {
                const msg = error.response?.data;
                setResponse(typeof msg === "string" ? msg : "Erro ao realizar login");
                setIsError(true)
            }
        } finally {
            setIsPosting(false);
        }
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-bege-claro px-4">
            <div className="w-full max-w-sm rounded-2xl border border-coral-suave/20 bg-pastel p-8 shadow-2xl
backdrop-blur-md">
                <h1 className="mb-8 text-center text-3xl text-black">Login</h1>

                <div className="flex flex-col gap-4">

                    <div className="flex flex-col gap-1">
                        <input
                            name="email"
                            type="email"
                            className="w-full rounded-md bg-white/45 p-2 focus:outline-white"
                            placeholder="Email"
                            value={formData.email}
                            onChange={(e) =>
                                setFormData({
                                    ...formData,
                                    [e.currentTarget.name]: e.currentTarget.value,
                                })
                            }
                        />
                        <span className="min-h-4.5 text-sm text-pink-500">
                            {formErrors.email}
                        </span>
                    </div>

                    <div className="flex flex-col gap-1">
                        <div className="relative">
                            <input
                                name="password"
                                type={inputType}
                                className="w-full rounded-md bg-white/45 p-2 pr-10 focus:outline-white"
                                placeholder="Senha"
                                value={formData.password}
                                onChange={(e) =>
                                    setFormData({
                                        ...formData,
                                        [e.currentTarget.name]: e.currentTarget.value,
                                    })
                                }
                            />

                            {inputType === "text" ? (
                                <EyeClosed
                                    className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-500"
                                    onMouseDown={handleMouseDown}
                                    onMouseUp={handleMouseUp}
                                />
                            ) : (
                                <Eye
                                    className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-500"
                                    onMouseDown={handleMouseDown}
                                    onMouseUp={handleMouseUp}
                                />
                            )}
                        </div>

                        <span className="min-h-4.5 text-sm text-pink-500">
                            {formErrors.password}
                        </span>
                    </div>

                    {response !== "" && (
                        <p className={`text-center text-sm ${isError ? "text-red-500" : "text-verde-claro"}`}>{response}</p>
                    )}

                    {isPosting ? (
                        <div className="flex items-center justify-center rounded-md bg-white/55 p-2">
                            <LoaderCircle className="animate-spin" />
                        </div>
                    ) : (
                        <button
                            type="button"
                            className="rounded-md bg-white/55 p-2 transition hover:bg-white/70 cursor-pointer"
                            onClick={handleButtonClick}
                        >
                            Entrar
                        </button>
                    )}
                </div>
            </div>
        </div>
    );
}