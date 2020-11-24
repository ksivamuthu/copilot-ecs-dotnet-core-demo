import { useEffect, useState } from 'react';
import "./tailwind.output.css"
import axios from 'axios';
import CoffeeIcons from './coffee-icons.svg';

const coffeeServiceBaseUrl = process.env.REACT_APP_COFFEE_SERVICE || '/coffee-service';

function CoffeeList() {
    const [coffees, setCoffees] = useState<{ coffeeName: string, coffeeId: string }[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const result = await axios(`${coffeeServiceBaseUrl}/api/coffee`);
            setCoffees(result.data);
        };

        fetchData();
    }, []);

    return (
        <div className="flex-row pt-6">
            <h2 className="text-xl leading-7 text-gray-900">
                Coffee Menu
            </h2>
            <div className="flex flex-wrap -mx-5 overflow-hidden">
                {coffees.map(coffee =>
                <div className="my-5 px-5 w-1/5 overflow-hidden">
                    <div className="flex flex-col items-center justify-center bg-white p-4 shadow rounded-lg">
                        <div className="inline-flex shadow-lg border border-gray-500 rounded-full overflow-hidden h-40 w-40">
                            <svg className="coffee-icon center w-full h-full p-8"><use href={CoffeeIcons + "#" + coffee.coffeeId}></use></svg>
                        </div>

                        <h2 className="mt-4 font-bold text-xl">{coffee.coffeeName}</h2>				
                    </div>
                </div>
                )}
            </div>
        </div>
    );
}

export default CoffeeList;
