import { useEffect, useState } from 'react';
import { IProductItem } from '../../../interfaces/products';
import { Link } from 'react-router-dom';
import { Button, Carousel } from 'antd';
import { PlusCircleFilled } from '@ant-design/icons';
import {API_URL, http_common} from "../../../env";

const ProductListPage = () => {
    const [list, setList] = useState<IProductItem[]>([]);

    useEffect(() => {
        http_common.get<IProductItem[]>("/api/Products")
            .then(resp => {
                setList(resp.data);
            });
    }, []);

    return (
        <>
            <p className='text-center text-3xl font-bold mb-5'>Products</p>
            <Link to={"/products/create"}>
                <Button type="primary" shape="round" icon={<PlusCircleFilled />} style={{ marginTop: 10, marginBottom: 20 }} />
            </Link>

            <div className='grid md:grid-cols-3 lg:grid-cols-4 gap-10'>
                {list.map(item =>
                    <div key={item.id} className='border rounded-lg overflow-hidden shadow-lg'>
                        <Carousel arrows infinite={false}>
                            {item.images.map((image, i) => (
                                <div key={i}>
                                    <img src={`${API_URL}/images/300_${image}`} alt={item.name} className='w-full h-48 object-cover' />
                                </div>
                            ))}
                        </Carousel>
                        <div className='p-4'>
                            <h3 className='text-xl font-semibold mb-2'>{item.name}</h3>
                            <p className='text-teal-800 font-bold text-xl'>{item.price}<span className='text-sm'>$</span></p>
                        </div>
                    </div>
                )}
            </div>
        </>
    );
}

export default ProductListPage;