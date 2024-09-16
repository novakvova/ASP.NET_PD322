import MainLayout from "./components/containers/default";
import {Route, Routes} from "react-router-dom";
import HomePage from "./components/home";

export default function App() {
    return (
        <>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    <Route index element={<HomePage />} />

                    {/* Using path="*"" means "match anything", so this route
                acts like a catch-all for URLs that we don't have explicit
                routes for. */}
                    {/*<Route path="*" element={<NoMatch />} />*/}
                </Route>
            </Routes>
        </>
    )
}
