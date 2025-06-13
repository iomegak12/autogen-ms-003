from mcp.server.fastmcp import FastMCP

import time
import signal
import sys

def signal_handler(sig, frame):
    print("Exiting gracefully...")
    sys.exit(0)
    
signal.signal(signal.SIGINT, signal_handler)

mcp = FastMCP(
    name="counter-r",
    timeout=30)

@mcp.tool()
def count_r(word: str) -> int:
    """Counts the number of occurrences of 'r' in the given word."""

    try:
        if not isinstance(word, str):
            return 0
    
        return word.lower().count('r')
    except Exception as e:
        print(f"Error counting 'r': {e}")
        return 0

if __name__ == "__main__":
    try:
        print("Starting FastMCP server...")
        
        mcp.run(transport="stdio")
        
    except Exception as e:
        print(f"An error occurred: {e}")
        time.sleep(1)
        print("Exiting FastMCP server...")