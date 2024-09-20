import {useEffect, useState} from "react";
import {ICategoryItem} from "./types.ts";
import {API_URL, http_common} from "../../env";
import {Link} from "react-router-dom";
import {DeleteDialog} from "../common/DeleteDialog.tsx";

const HomePage = () => {
    const [list, setList] = useState<ICategoryItem[]>([]);

    useEffect(() => {
        http_common.get<ICategoryItem[]>("/api/categories")
            .then(resp => {
                //console.log("list", resp);
                setList(resp.data);
            });
    }, []);

    const hamdleDelete = async (id: number) => {
        //console.log("Delete id", id);
        try {
            await http_common.delete("/api/categories/" + id);
            setList(list.filter(item => item.id !== id));
        } catch {
            //toast
        }

    }

    return (
        <>
            <h1 className={"text-center text-3xl font-bold tracking-tight text-gray-900 mb-2"}>Категорії</h1>
            <div>
                <Link to="/create"
                      className="inline-flex items-center px-3 py-2 text-sm font-medium text-center text-white bg-blue-700 rounded-lg hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                    Додати
                </Link>
            </div>
            <div className='grid md:grid-cols-3 lg:grid-cols-4 gap-4'>
                {list.map(item =>
                    <div
                        className="max-w-sm bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
                        <a href="#">
                            <img className="rounded-t-lg" src={API_URL + "/images/300_" + item.image} alt=""/>
                        </a>
                        <div className="p-5">
                            <a href="#">
                                <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">{item.name}</h5>
                            </a>
                            <p className="mb-3 font-normal text-gray-700 dark:text-gray-400">{item.description}</p>
                            <DeleteDialog title={"Ви впевнені?"}
                                          description={`Дійсно бажаєте видалити '${item.name}'?`}
                                          onSubmit={() => hamdleDelete(item.id)}/>
                        </div>
                    </div>
                )}
            </div>

        </>
    );
}

export default HomePage;