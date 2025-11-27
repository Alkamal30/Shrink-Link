import { useCallback, useState } from 'react';
import { shrinkLink } from '../services/linkService';
import './LinkInput.css';

const LinkInput: React.FC = () => {
    const [value, setValue] = useState("");
    const [result, setResult] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleClick = useCallback(async () => {
        const trimmedValue = value.trim();

        if(!trimmedValue) {
        setError("Input field is empty!");
        return;
        }

        setLoading(true);
        setError("");
        setResult("");

        try {
        const shrinkedLink = await shrinkLink(trimmedValue);
        setResult(shrinkedLink);
        } catch(err: unknown) {
        setError(err instanceof Error ? err.message : "Unknown error");
        } finally {
        setLoading(false);
        }
    }, [value]);

    return (
        <div className="link-input">
            <input className="link-input__field" type="text" placeholder="www.your-link.com"
                value={value} onChange={(e) => setValue(e.target.value)}/>
            <button className="link-input__action" onClick={handleClick} disabled={loading || !value.trim()}>
                Shrink
            </button>
            <p className="link-input__tip">
                Enter your link and click button to shorten
            </p>
            {result && (<a href={result}>{result}</a>)}
            {error && (<p style={{color: 'red'}}>{error}</p>)}
        </div>
    )
};

export default LinkInput;